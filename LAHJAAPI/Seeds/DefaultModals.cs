using LAHJAAPI.Services2;
using LAHJAAPI.Data;
using LAHJAAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LAHJAAPI.Utilities.Seeds
{
    public static class DefaultModals
    {
        public static async Task SeedAsync(DataContext context)
        {
            //context.ModelGateways.ExecuteDelete();
            if (await context.ModelGateways.FirstOrDefaultAsync(p => p.Name == "web") == null)
            {
                var Token = TokenService.GenerateSecureToken();

                await context.ModelGateways.AddRangeAsync([new ModelGateway
                {
                    Name = "Core",
                    Url = "https://modelspeech.onrender.com",
                    Token = "https://modelspeech.onrender.com",
                    IsDefault = true
                },
                    new ModelGateway
                {
                    Name = "huggingface",
                    Url = "https://huggingface.co/wasmdashai",
                    Token = Token,
                    IsDefault = false
                }
                ]);


                await context.ModelAis.AddAsync(new ModelAi
                {
                    Name = "Wasm Speeker",
                    AbsolutePath = "wasm-speeker",
                    Token = Token,
                });


                await context.SaveChangesAsync();
            }



        }
    }
}