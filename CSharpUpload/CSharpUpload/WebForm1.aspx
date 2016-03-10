<%@ Page language="c#" Codebehind="WebForm1.aspx.cs" AutoEventWireup="false" Inherits="CSharpUpload.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>WebForm1</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </HEAD>
  <body MS_POSITIONING="GridLayout">
<form id="Form1" method="post" enctype="multipart/form-data" runat="server">
<INPUT type=file id=musicFilesToUpload name=musicFilesToUpload[] runat="server" multiple="multiple" >
<br>
<input type="submit" id="Submit1" value="Upload" runat="server" NAME="Submit1">
</form>

<form runat="server">
<asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
</form>

	
  </body>
</HTML>