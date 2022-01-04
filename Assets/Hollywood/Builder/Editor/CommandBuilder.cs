using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Hollywood.Builder.Editor
{
    internal static class CommandBuilder
    {
        /// <summary>
        /// 换行符
        /// </summary>
        private static readonly string EOL = Environment.NewLine;

        /// <summary>
        /// 不需要展示出来的内容
        /// </summary>
        /// <value></value>
        private static readonly string[] Secrets =
            {"androidKeystorePass", "androidKeyaliasName", "androidKeyaliasPass"};

        private static Dictionary<string, string> Defaults = new Dictionary<string, string> {
            { "projectPath", Environment.CurrentDirectory },
            { "buildTarget", BuildTarget.Android.ToString() },
            { "customBuildPath", $"build/{BuildTarget.Android.ToString()}" },
            { "version", Application.version },
            { "androidVersionCode", PlayerSettings.Android.bundleVersionCode.ToString() },
            { "androidKeystoreName", "release.keystore" },
            { "androidKeystorePass", "******" },
            { "androidKeyaliasName", "******" },
            { "androidKeyaliasPass", "******" },
            { "androidTargetSdkVersion", "AndroidApiLevel31" },
        };

        // 隐式使用
        [JetBrains.Annotations.UsedImplicitly]
        public static void BuildOptions()
        {
            // 从参数中收集值
            Dictionary<string, string> options = GetValidatedOptions();

            // 为此次构建设置版本号
            string version = options["buildVersion"];
            PlayerSettings.bundleVersion = version;
            PlayerSettings.macOS.buildNumber = version;
            // 如果版本只有一个点
            while (version.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length < 4)
                version += ".0";
            PlayerSettings.WSA.packageVersion = new Version(version);

            // 设置对应的构建目标
            var buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), options["buildTarget"]);
            switch (buildTarget)
            {
                case BuildTarget.Android:
                    {
                        PlayerSettings.Android.bundleVersionCode = int.Parse(options["androidVersionCode"]);
                        EditorUserBuildSettings.buildAppBundle = options["customBuildPath"].EndsWith(".aab");
                        if (options.TryGetValue("androidKeystoreName", out string keystoreName) &&
                            !string.IsNullOrEmpty(keystoreName))
                            PlayerSettings.Android.keystoreName = keystoreName;
                        if (options.TryGetValue("androidKeystorePass", out string keystorePass) &&
                            !string.IsNullOrEmpty(keystorePass))
                            PlayerSettings.Android.keystorePass = keystorePass;
                        if (options.TryGetValue("androidKeyaliasName", out string keyaliasName) &&
                            !string.IsNullOrEmpty(keyaliasName))
                            PlayerSettings.Android.keyaliasName = keyaliasName;
                        if (options.TryGetValue("androidKeyaliasPass", out string keyaliasPass) &&
                            !string.IsNullOrEmpty(keyaliasPass))
                            PlayerSettings.Android.keyaliasPass = keyaliasPass;
                        break;
                    }
                case BuildTarget.StandaloneOSX:
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    if (!options["customBuildPath"].EndsWith(".exe"))
                        options["customBuildPath"] = options["customBuildPath"] + "/triple.exe";
                    break;
                case BuildTarget.WSAPlayer:
                    EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.XAML;
                    break;
            }

            // 自定义构建
            Build(buildTarget, options["customBuildPath"]);
        }

        /// <summary>
        /// 验证输入内容
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetValidatedOptions()
        {
            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

            if (!validatedOptions.TryGetValue("projectPath", out string _))
            {
                Console.WriteLine("Missing argument -projectPath");
                EditorApplication.Exit(110);
            }

            if (!validatedOptions.TryGetValue("buildTarget", out string buildTarget))
            {
                Console.WriteLine("Missing argument -buildTarget");
                EditorApplication.Exit(120);
            }

            if (!Enum.IsDefined(typeof(BuildTarget), buildTarget ?? string.Empty))
            {
                EditorApplication.Exit(121);
            }

            if (validatedOptions.TryGetValue("buildPath", out string buildPath))
            {
                validatedOptions["customBuildPath"] = buildPath;
            }

            if (!validatedOptions.TryGetValue("customBuildPath", out string _))
            {
                Console.WriteLine("Missing argument -customBuildPath");
                EditorApplication.Exit(130);
            }

            return validatedOptions;
        }

        /// <summary>
        /// 分析命令行参数
        /// </summary>
        /// <param name="arguments"></param>
        private static void ParseCommandLineArguments(out Dictionary<string, string> arguments)
        {
            arguments = new Dictionary<string, string>(Defaults);
            string[] args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                $"{EOL}" +
                $"###########################{EOL}" +
                $"#         分析参数        #{EOL}" +
                $"###########################{EOL}" +
                $"{EOL}"
            );

            // 提取具有值的标志
            const char dash = '-';
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                var isFlag = args[current].StartsWith(dash.ToString());
                if (!isFlag)
                {
                    continue;
                }
                var flag = args[current].TrimStart(dash);
                var flagHasValue = next < args.Length && !args[next].StartsWith(dash.ToString());
                var value = flagHasValue ? args[next].TrimStart(dash) : "";
                if (Defaults.ContainsKey(flag) && (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)))
                {
                    // 如果没有值就采用默认的
                    value = Defaults[flag];
                }
                var displayValue = Secrets.Contains(flag) ? "*密密密*" : $"\"{value}\"";
                Console.WriteLine($"找到的标志为 \"{flag}\" 其值为 {displayValue}.");
                if (!arguments.ContainsKey(flag))
                {
                    arguments.Add(flag, value);
                }
                else
                {
                    arguments[flag] = value;
                }
            }
        }

        /// <summary>
        /// 启动构建
        /// </summary>
        /// <param name="buildTarget"></param>
        /// <param name="filePath"></param>
        private static void Build(BuildTarget buildTarget, string filePath)
        {
            var scenes = (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                target = buildTarget,
                locationPathName = filePath,
                // targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget),
                // options = UnityEditor.BuildOptions.Development
            };

            var buildSummary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
            ReportSummary(buildSummary);
            ExitWithResult(buildSummary.result);
        }

        /// <summary>
        /// 上报结果
        /// </summary>
        /// <param name="summary"></param>
        private static void ReportSummary(BuildSummary summary)
        {
            Console.WriteLine(
                $"{EOL}" +
                $"###########################{EOL}" +
                $"#         构建结果        #{EOL}" +
                $"###########################{EOL}" +
                $"{EOL}" +
                $"持续时间: {summary.totalTime.ToString()}{EOL}" +
                $"警告数量: {summary.totalWarnings.ToString()}{EOL}" +
                $"错误数量: {summary.totalErrors.ToString()}{EOL}" +
                $"累计大小: {summary.totalSize.ToString()} bytes{EOL}" +
                $"{EOL}"
            );
        }

        /// <summary>
        /// 返回结果到命名行
        /// </summary>
        /// <param name="result"></param>
        private static void ExitWithResult(BuildResult result)
        {
            switch (result)
            {
                case BuildResult.Succeeded:
                    Console.WriteLine("构建成功!");
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Failed:
                    Console.WriteLine("构建失败!");
                    EditorApplication.Exit(101);
                    break;
                case BuildResult.Cancelled:
                    Console.WriteLine("构建取消!");
                    EditorApplication.Exit(102);
                    break;
                case BuildResult.Unknown:
                default:
                    Console.WriteLine("构建未知!");
                    EditorApplication.Exit(103);
                    break;
            }
        }
    }
}