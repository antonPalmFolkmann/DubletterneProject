public static class SeedExtensions
{
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LearningContext>();
            var repository = scope.ServiceProvider.GetRequiredService<IResourceRepository>();

            await SeedResourcesAsync(context);
        }
        return host;
    }

    private static async Task SeedResourcesAsync(LearningContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Resources.AnyAsync())
        {
            var text1 = new TextParagraph("Text1");
            var text2 = new TextParagraph("Text2");
            var text3 = new TextParagraph("Text3");
            var text4 = new TextParagraph("Text4");
            var text5 = new TextParagraph("Text5");
            var text6 = new TextParagraph("Text6"); 

            var hjelmgaard = new User {FirstName = "Alexander", LastName = "Hjelmgaard", UserName = "hjel", Created = DateTime.Today, Updated = DateTime.Now, Email = "hjel@itu.dk"};
            var schiller = new User {FirstName = "Alexander", LastName = "schiller", UserName = "alesc", Created = DateTime.Today, Updated = DateTime.Now, Email = "alesc@itu.dk"};
            var anton = new User {FirstName = "Anton", LastName = "Folkmann", UserName = "anpf", Created = DateTime.Today, Updated = DateTime.Now, Email = "anpf@itu.dk"};
            var frederik = new User {FirstName = "Frederik", LastName = "Thomsen", UserName = "fret",Created = DateTime.Today, Updated = DateTime.Now, Email = "fret@itu.dk"};
            var crawley = new User {FirstName = "Nicklas", LastName = "Jørgensen", UserName = "ncjo",Created = DateTime.Today, Updated = DateTime.Now, Email = "ncjo@itu.dk"};
            var askjaer = new User {FirstName = "Nicklas", LastName = "Askjaer", UserName = "nias",Created = DateTime.Today, Updated = DateTime.Now, Email = "nias@itu.dk"};
            var users = new[] {hjelmgaard, schiller, anton, frederik, crawley, askjaer};

            var resource1 = new Resource { User = hjelmgaard.UserName, Created = DateTime.Today, Title = "Superman", TextParagraphs = new List<TextParagraph>{text1, text2, text3}, ImageUrl = "http://image.com"};
            var resource2 = new Resource { User = schiller.UserName, Created = DateTime.Today, Title = "Spiderman", TextParagraphs = new List<TextParagraph>{text4, text5, text6}, ImageUrl = "http://image.com"};
            var resource3 = new Resource { User = anton.UserName, Created = DateTime.Today, Title = "Batman", TextParagraphs = new List<TextParagraph>{text1, text4, text2}, ImageUrl = "http://image.com"};
            var resource4 = new Resource { User = frederik.UserName, Created = DateTime.Today, Title = "Ironman", TextParagraphs = new List<TextParagraph>{text6, text5, text1}, ImageUrl = "http://image.com"};
            var resource5 = new Resource { User = crawley.UserName, Created = DateTime.Today, Title = "Aquaman", TextParagraphs = new List<TextParagraph>{text5, text6, text2}, ImageUrl = "http://image.com"};
            var resource6 = new Resource { User = askjaer.UserName, Created = DateTime.Today, Title = "Ant-Man", TextParagraphs = new List<TextParagraph>{text4, text2, text3}, ImageUrl = "http://image.com"};

            hjelmgaard.Resources.Add(resource1);
            schiller.Resources.Add(resource2);
            anton.Resources.Add(resource3);
            frederik.Resources.Add(resource4);
            crawley.Resources.Add(resource5);
            askjaer.Resources.Add(resource6);

            context.Users.AddRange(users);
            //context.Resources.AddRange();

            await context.SaveChangesAsync();
        }
    }
}