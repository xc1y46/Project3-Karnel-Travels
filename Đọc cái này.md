# Lúc tải project và chạy file sql nhớ sửa lại connection string của cái dataContext trong file config

# Nếu lúc pull git cái project adm mà bị lỗi Package NuGet CodeDom.Provider.DotNetCompiler.... thì fix như sau
	1. Chuột phải project, chọn unload project
	2. Chuột phải lại project, chọn edit file project (file .csproj)
	3. Kéo xuống gần cuối xóa cái này 
		<Target Name="EnsureNuGetPackageBuildImports" BeforeTargets="PrepareForBuild">
			<PropertyGroup>
				<ErrorText>This project references NuGet package(s) that are missing on this computer. Use NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is {0}.</ErrorText>
			</PropertyGroup>
			<Error Condition="!Exists('..\packages\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.2.0.1\build\net46\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.props')" Text="$([System.String]::Format('$(ErrorText)', '..\packages\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.2.0.1\build\net46\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.props'))" />
		</Target>
	4. Save rồi chuột phải project rồi reload

# Nếu lúc pull git cái project adm mà bị lỗi phiên bản Newtonsoft.Json thì fix như sau

	1. Tools -> NuGet package manager -> Package manager console
	2. Gõ update-package Newtonsoft.Json -reinstall rồi enter
	(Nếu muốn cụ thể phiên bản thì gõ -version 6.0.0 trước -reinstall)
	3. Đợi chạy xong thì vào web.config, xóa phần này
		<dependentAssembly>
			<assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
			<bindingRedirect oldVersion="0.0.0.0-6.0.0.0" newVersion="6.0.0.0" />
		</dependentAssembly>
