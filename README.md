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

2. 推送该文件并手动运行该`Action`等待片刻得到一个`Unity_v20XX.X.XXXX.alf`并下载保存解压
3. 访问[手动激活 Unity 许可证](https://license.unity3d.com/manual)
4. 上传刚刚得到的`.alf`文件
   1. 可能会有 _serial has reached the maximum number of activations._ 这个问题的出现, 目前没有好的解决方案.
   2. 我的解决方案是创建了一个新的账号, 这个账号不在个人电脑上操作.
5. 得到一个 `Unity_v20XX.x.ulf` 文件, 里面的内容便是.

> 注意：更改 Unity 版本时，您可能需要重复相同的过程。

## GITHUB_TOKEN

1. 什么都不勾选试试看
