# 添加 SuperEdit 到文件资源管理器的上下文菜单 , 通过管理员权限运行

# 定义编辑器的路径
$editorPath = "A:\Scoop\apps\superedit\current\SuperEdit.exe"



# 检查编辑器路径是否存在
if (-Not (Test-Path $editorPath)) {
    Write-Host "编辑器路径无效：$editorPath" -ForegroundColor Red
    exit
}

# 注册表路径
$regPath = "计算机\HKEY_CLASSES_ROOT\*\shell\SuperEdit"
$commandPath = "$regPath\command"

# 创建上下文菜单键
New-Item -Path $regPath -Force | Out-Null
Set-ItemProperty -Path $regPath -Name "(Default)" -Value "用 SuperEdit 打开"

# 添加图标（可选）
Set-ItemProperty -Path $regPath -Name "Icon" -Value $editorPath

# 设置命令
New-Item -Path $commandPath -Force | Out-Null
Set-ItemProperty -Path $commandPath -Name "(Default)" -Value "`"$editorPath`" `"%1`""

Write-Host "上下文菜单已成功添加。" -ForegroundColor Green
