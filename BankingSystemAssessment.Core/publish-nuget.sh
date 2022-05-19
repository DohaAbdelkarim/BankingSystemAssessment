dotnet nuget update source github --source https://nuget.pkg.github.com/DohaAbdelkarim/index.json --username DohaAbdelkarim --password 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 --store-password-in-clear-text

dotnet build BankingSystemAssessment.Core.csproj
dotnet nuget push --source "github" --api-key az bin/Debug/BankingSystemAssessment.Core.1.0.0.nupkg