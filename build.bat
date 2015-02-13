rem C:\Program Files (x86)\MySQL\Connector.NET 6.9\Assemblies\v4.0 rem v4.0.30319
where /Q csc || set path=%path%;c:\Windows\Microsoft.NET\Framework\v3.5\;
csc /reference:".\lib\mysql-connector-v2.0\MySql.Data.dll"^
    /utf8output^
	/nologo^
	/win32icon:assets\app.ico^
	/target:winexe^
	/out:Thuraiya.exe^
	/recurse:src\*.cs
pause