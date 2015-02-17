rem C:\Program Files (x86)\MySQL\Connector.NET 6.9\Assemblies\v4.0 rem v4.0.30319
where /Q csc || set path=%path%;c:\Windows\Microsoft.NET\Framework\v3.5\;
csc /reference:lib\mysql-connector-v2.0\MySql.Data.dll^
    /reference:lib\sqlite\x86\System.Data.SQLite.dll^
    /utf8output^
    /nologo /platform:x86^
    /win32icon:assets\app.ico^
    /target:exe^
    /out:bin\Thuraiya.exe^
    /recurse:src\*.cs
rem    /nostdlib+ /errorreport:prompt
rem copy lib\mysql-connector-v2.0\*.dll bin\
rem copy lib\sqlite\x86\SQLite.Interop.dll bin\
rem copy lib\sqlite\x86\System.Data.SQLite.dll bin\
rem mkdir bin\assets
rem copy assets bin\assets\