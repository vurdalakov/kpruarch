# KpRuArch

'KpRuArch' command-line tool downloads [Komsomolskaya Pravda](https://en.wikipedia.org/wiki/Komsomolskaya_Pravda) newspaper [PDF archive](http://www.kp.ru/vday/).

##### Syntax

```
kpruarch <id> <-d | -l> -from:1941-06 -to:1954-05 [-o:outdir] [-silent]
```

You get an `id` when you make a purchase at (http://www.kp.ru/vday/).

In `-d` mode application downloads PDF files, and in `-l` mode just lists PDF file URLs (no download happens).

`-from` and `-to` options define first and last months to download.

Optional `-o` option sets the output directory (default is application directory).

Optional `-silent` option tells application not to print anything to stdout in `-d` mode; check exit code for result.

**Examples:**

```
kpruarch 7ac813e090af08b5db74 -d -from:1941-06 -to:1945-05 -o:c:\temp\kp.ru
```
```
kpruarch 7ac813e090af08b5db74 -l -from:1941-06 -to:1954-05 > urls.txt
```

##### License

Copyright © 2015 [Vurdalakov](http://www.vurdalakov.net).

Project is distributed under the [MIT license](http://opensource.org/licenses/MIT).
