// Nuget : Azure.Identity
using Azure.Identity;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Ajoute les pages Razor à l'application.
// Permet d’afficher et gérer les pages .cshtml.
builder.Services.AddRazorPages();

// Récupère l’URL du Key Vault à partir d’une variable d’environnement.
// Cette valeur est fournie par Azure lors du déploiement.
string KeyVaultURI = Environment.GetEnvironmentVariable("KeyVaultUri");
Console.WriteLine("KeyVault URI : " + KeyVaultURI);

// Ajoute Azure Key Vault à la configuration de l'application.
// Permet de charger automatiquement les secrets depuis Key Vault.
// DefaultAzureCredential utilise l'identité managée en production.
builder.Configuration.AddAzureKeyVault(
    new Uri(KeyVaultURI),
    new DefaultAzureCredential());

var app = builder.Build();

// Configure le pipeline HTTP pour l'environnement de production.
// Active la page d’erreur personnalisée si on n’est pas en mode développement.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();
// Mappe les pages Razor et relie automatiquement les fichiers statiques.
// Permet de servir les pages .cshtml et leurs ressources.
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
// Lance l'application web.
app.Run();
