Encoding Issue Finder
=====================

Traverses the given directory path in search for file encoding issues caused by special characters of particular encoding system in relation to UTF-8.

Status
======

Currently checks against `macintosh` (10000) encoding issues only.

Usage
=====

Binary file is in the `bin\Release\` folder.

Sample usage:
`NonUtfDetector.exe D:\project php,txt,xml,dist`

Compiled with .NET 3.5 therefore should be compatible with OSX/Linux (although not tested):

`mono NonUtfDetector.exe /opt/myproject php,txt,xml,dist`

