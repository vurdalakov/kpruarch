namespace Vurdalakov
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            if (_commandLineParser.FileNames.Length != 1)
            {
                Help();
            }

            _id = _commandLineParser.FileNames[0];

            _directory = _commandLineParser.GetOptionString("output", ApplicationDirectory);
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            var from = _commandLineParser.GetOptionString("from", "1941-06");
            var to = _commandLineParser.GetOptionString("to", "1945-05");

            Int32 fromYear, fromMonth, toYear, toMonth;
            if (!ParseMonth(from, out fromYear, out fromMonth) || !ParseMonth(to, out toYear, out toMonth))
            {
                Help();
                return -1; // to avoid error CS0165
            }

            for (var month = fromMonth; month <= 12; month++)
            {
                DownloadMonth(fromYear, month);
            }

            for (var year = fromYear + 1; year < toYear; year++)
            {
                for (var month = 1; month <= 12; month++)
                {
                    DownloadMonth(year, month);
                }
            }

            if (fromYear != toYear)
            {
                for (var month = 1; month <= toMonth; month++)
                {
                    DownloadMonth(toYear, month);
                }
            }

            return 0;
        }

        private String _id;
        private String _directory;

        private WebClient _webClient = new WebClient();

        private void DownloadMonth(Int32 year, Int32 month)
        {
            Print("{0}-{1:D2}", year, month);

            var url = String.Format("http://www.kp.ru/arch/daily/great_war/{0}/{1:D2}/{2}/", year, month, _id);
            
            var html = _webClient.DownloadString(url);

            var matches = Regex.Matches(html, @"""(http://pdf-download.kp.ru/api/pdf.bin/.*/(\d\d\d\d)/(\d\d)/(\d\d)/)""");
            if (0 == matches.Count)
            {
                Console.WriteLine("No matches!");
            }

            foreach (Match match in matches)
            {
                if (5 == match.Groups.Count)
                {
                    var fileUrl = match.Groups[1].Value;
                    var fileName = String.Format("kp_{0}_{1:D2}_{2:D2}.pdf", match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value);
                    var filePath = Path.Combine(_directory, fileName);

                    if (File.Exists(filePath))
                    {
                        continue;
                    }

                    Print("{0}", fileName);
                    _webClient.DownloadFile(fileUrl, filePath);
                }
            }
        }

        private Boolean ParseMonth(String yearMonth, out Int32 year, out Int32 month)
        {
            var match = Regex.Match(yearMonth, @"(\d\d\d\d)-(\d\d)");
            
            if (match.Groups.Count != 3)
            {
                year = -1;
                month = -1;
                return false;
            }

            year = Int32.Parse(match.Groups[1].Value);
            month = Int32.Parse(match.Groups[2].Value);

            return true;
        }

        protected override void Help()
        {
            Environment.Exit(-1);
        }
    }
}
