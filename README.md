# IISIPConfigTool
A tool for IIS6/7 to config IP allow/deny 

![toolimage](http://www.hdrorz.name/wp-content/uploads/2017/01/IISConfigTool.jpg)

因为有这么一个批量修改IIS的IP限制的需求，还要兼容IIS6和7，就诞生了这么一个工具。
IIS6使用System.DirectoryServices实现，IIS7默认使用Microsoft.Web.Administration实现，可以修改为使用AppCmd命令行。

对于一个IP段，实现了计算子网的功能，但是用户体验不好，默认关闭，可以配置config文件打开这个功能。

## 博客
[IIS的IP限制管理工具](http://www.hdrorz.name/archives/110)

## 参考
[Setting IP Security Using System.DirectoryServices](https://msdn.microsoft.com/en-us/library/ms524322%28v=vs.90%29.aspx)
[Appcmd.exe (IIS 7)](https://technet.microsoft.com/en-us/library/cc772200.aspx)
