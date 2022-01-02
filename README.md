# triple

```yml
# 正式用户
UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
UNITY_SERIAL: ${{ secrets.UNITY_SERIAL }}
# 免费用户
UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
```

## UNITY_LICENSE

1. 创建一个文件, 加入以下代码, 并放置到 `.github/workflows/getManualLicenseFile.yml`

   ```yml
   name: 获取激活文件
   on:
     workflow_dispatch: {}
   jobs:
     activation:
       name: 请求手动激活文件 🔑
       runs-on: ubuntu-latest
       steps:
         # 请求手动激活文件
         - name: 请求手动激活文件
           id: getManualLicenseFile
           uses: game-ci/unity-request-activation-file@v2
         # Upload artifact (Unity_v20XX.X.XXXX.alf)
         - name: 导出
           uses: actions/upload-artifact@v2
           with:
             name: ${{ steps.getManualLicenseFile.outputs.filePath }}
             path: ${{ steps.getManualLicenseFile.outputs.filePath }}
   ```

2. 推送该文件并手动运行该`Action`得到一个文件并下载保存
3. 访问[手动激活 Unity 许可证](https://license.unity3d.com/manual)
4. 上传刚刚得到的文件
5.
