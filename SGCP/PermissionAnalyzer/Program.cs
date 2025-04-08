
using SGCP.PermissionAnalyzer;

var analyzer = new PermissionAnalysisService(strictMode: args.Contains("--strict"));
analyzer.RunAnalysis();