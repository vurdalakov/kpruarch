namespace Vurdalakov
{
    using System;

    public class Application : DosToolsApplication
    {
        protected override Int32 Execute()
        {
            return 0;
        }

        protected override void Help()
        {
            Environment.Exit(-1);
        }
    }
}
