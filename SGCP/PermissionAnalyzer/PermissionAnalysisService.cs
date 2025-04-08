using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SGCP.PermissionAnalyzer
{
    public class PermissionAnalysisService
    {
        private readonly string _controllersPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "SGCP", "Controllers");
        private readonly bool _strictMode;
        private readonly StringBuilder _report = new();

        public PermissionAnalysisService(bool strictMode = false)
        {
            _strictMode = strictMode;
        }

        public void RunAnalysis()
        {
            var files = Directory.GetFiles(_controllersPath, "*.cs", SearchOption.AllDirectories);
            _report.AppendLine("# Endpoint Permission Report\n");

            foreach (var file in files)
            {
                var code = File.ReadAllText(file);
                var tree = CSharpSyntaxTree.ParseText(code);
                var root = tree.GetRoot();

                var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classNode == null) continue;

                var className = classNode.Identifier.Text;
                var classAuthorizeAttr = classNode.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .FirstOrDefault(attr => attr.Name.ToString().Contains("Authorize"));

                string classAuthorizeInfo = "❌";
                if (classAuthorizeAttr != null)
                {
                    var rolesArg = classAuthorizeAttr.ArgumentList?.Arguments
                        .FirstOrDefault(arg => arg.ToString().Contains("Roles"));
                    classAuthorizeInfo = rolesArg != null ? $"✅ Roles = {rolesArg}" : "✅";
                }

                _report.AppendLine($"## 📄 {className}");
                _report.AppendLine($"Class-level [Authorize]: {classAuthorizeInfo}");

                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    .Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword));

                foreach (var method in methods)
                {
                    var methodName = method.Identifier.Text;
                    var attributes = method.AttributeLists.SelectMany(al => al.Attributes).ToList();

                    var authAttr = attributes.FirstOrDefault(a => a.Name.ToString().Contains("Authorize"));
                    var hasAuthorize = authAttr != null;
                    var hasPermission = attributes.FirstOrDefault(a => a.Name.ToString().Contains("HasPermission"));
                    var permissionValue = hasPermission?.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"');

                    var authInfo = "❌";
                    if (hasAuthorize)
                    {
                        var rolesArg = authAttr.ArgumentList?.Arguments
                            .FirstOrDefault(arg => arg.ToString().Contains("Roles"));
                        authInfo = rolesArg != null ? $"✅ Roles = {rolesArg}" : "✅";
                    }

                    var missingPermission = hasPermission == null;

                    if (_strictMode && !missingPermission)
                        continue;

                    _report.AppendLine($"- `{methodName}()`");
                    _report.AppendLine($"  - [Authorize]: {authInfo}");
                    _report.AppendLine($"  - [HasPermission]: {(hasPermission != null ? $"✅ `{permissionValue}`" : "❌")}");
                    if (missingPermission)
                        _report.AppendLine("  - ⚠️ FALTA `[HasPermission]`");
                }

                _report.AppendLine();
            }

            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputFolder);
            string outputPath = Path.Combine(outputFolder, "permissions_report.md");

            File.WriteAllText(outputPath, _report.ToString());
            Console.WriteLine($"✅ Report generated at: {outputPath}");

            // Intentar abrir el archivo en Windows
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = outputPath,
                    UseShellExecute = true
                });
            }
            catch { }
        }
    }
}