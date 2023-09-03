using Biblioteka.Data;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;

namespace Biblioteka
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BibliotekaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BibliotekaContext>>())
                )
            {
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange
                    (
                        new Microsoft.AspNetCore.Identity.IdentityRole { Name = "Bibliotekarz", NormalizedName = "BIBLOTEKARZ" },
                        new Microsoft.AspNetCore.Identity.IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
                    );
                    context.SaveChanges();
                }
                context.Autor.ExecuteDelete();
                context.SaveChanges ();
                
                if (!context.Autor.Any()) 
                {
                    var autor1 = new Models.Autor
                    {

                        Nazwa = "John Ronald Reuel Tolkien",
                        Description = " brytyjski pisarz oraz profesor filologii klasycznej i literatury staroangielskiej na University of Oxford.",
                        DataSmierci = new DateTime(1973, 9, 2),
                        DataUrodzenia = new DateTime(1892, 3, 1)
                    };
                    var autor2 = new Models.Autor
                    {

                        Nazwa = "Robert C. Martin",
                        Description = "Jest konsultantem o międzynarodowaj renomie w branży oprogramowania",
                        DataUrodzenia = new DateTime(1963, 4, 1),
                        DataSmierci = null
                    };
                    var autor3 = new Models.Autor
                    {

                        Nazwa = "Thomas Nield",
                        Description = "Ten autor nie ma jeszcze opisu",
                        DataSmierci = null,
                        DataUrodzenia = null
                    };
                    context.Autor.AddRange ( autor1,autor2,autor3 );
                    context.SaveChanges();
                }
                
                if(!context.Status.Any())
                {
                    context.Status.AddRange
                    (
                        new Models.Status {  Nazwa = "Zakończone" },
                        new Models.Status {  Nazwa = "Trwa" },
                        new Models.Status {  Nazwa = "Spóżnienie" }
                    );
                    context.SaveChanges();
                }
                
                if (!context.LibraryUser.Any()) 
                {
                    context.LibraryUser.AddRange
                    (
                        new Models.LibraryUser
                        {
                            
                            Imie = "Dezyderiusz",
                            Nazwisko = "Mops",
                            Adres = "Sosnowiec 23"
                        },
                        new Models.LibraryUser
                        {
                            
                            Imie = "Adrian",
                            Nazwisko = "Krawczyk",
                            Adres = "Warszawa 23"
                        },
                        new Models.LibraryUser
                        {
                            
                            Imie = "Paulina",
                            Nazwisko = "Mieścicka",
                            Adres = "Zbuczyn 23"
                        },
                        new Models.LibraryUser
                        {
                            
                            Imie = "Jakub",
                            Nazwisko = "Lis",
                            Adres = "Kotuń 23"
                        }
                    );
                    context.SaveChanges();
                }
                context.Zasob.ExecuteDelete();
                context.SaveChanges();
                //var Autorzy = context.Autor.ToList();
                

                if (!context.Zasob.Any())
                {
                    var z1 = new Models.Zasob
                    {
                        Tytul = "Niedokończone opowieści Śródziemia i Numenoru",
                        ISBN = "9780261102156",
                        Autorzy = new List<Models.Autor>() { context.Autor.ToList().Find(x => x.Nazwa == "John Ronald Reuel Tolkien") },
                    };
                    var z2 = new Models.Zasob
                    {

                        Tytul = "Czysty Kod",
                        ISBN = "9788383223445",
                        Autorzy = new List<Models.Autor>() { context.Autor.ToList().Find(x => x.Nazwa == "Robert C. Martin") }
                    };
                    var z3 = new Models.Zasob
                    {

                        Tytul = "Podstawy matematyki w data science",
                        ISBN = "9788383220147",
                        Autorzy = new List<Models.Autor>() { context.Autor.ToList().Find(x => x.Nazwa == "Thomas Nield") }
                    };
                    context.Zasob.AddRange
                    (  z1,z2,z3);
                    context.SaveChanges();
                }
                
                if (!context.Zbior.Any()) 
                {
                    context.Zbior.AddRange
                    (
                        new Models.Zbior
                        {
                            
                            CzyDostepny = true,
                            Zasob = context.Zasob.ToList().Find(x => x.ISBN == "9780261102156")
                        },
                        new Models.Zbior
                        {
                            
                            CzyDostepny = false,
                            Zasob = context.Zasob.ToList().Find(x => x.ISBN == "9780261102156")
                        },
                        new Models.Zbior
                        {
                            
                            CzyDostepny = true,
                            Zasob = context.Zasob.ToList().Find(x => x.ISBN == "9788383223445")
                        },
                        new Models.Zbior
                        {
                            
                            CzyDostepny = false,
                            Zasob = context.Zasob.ToList().Find(x => x.ISBN == "9788383223445")
                        },
                        new Models.Zbior
                        {
                            
                            CzyDostepny = true,
                            Zasob = context.Zasob.ToList().Find(x => x.ISBN == "9788383220147")
                        },
                        new Models.Zbior
                        {
                            
                            CzyDostepny = false,
                            Zasob = context.Zasob.ToList().Find(x => x.ISBN == "9788383220147")
                        }
                    );
                    context.SaveChanges();
                }
                context.Wyporzyczenie.ExecuteDelete();
                context.SaveChanges();
                if(!context.Wyporzyczenie.Any()) 
                {
                    context.Wyporzyczenie.AddRange
                    (
                        new Models.Wyporzyczenie
                        {
                            
                            Wyporzyczajacy = context.LibraryUser.ToList().Find(x => x.Nazwisko=="Mops"),
                            WyporzyczanyZbior = context.Zbior.Include(x => x.Zasob).ToList().Find(x => x.CzyDostepny==true && x.Zasob.ISBN=="9780261102156"),
                            ObecnyStatus = context.Status.ToList().Find(x=> x.Nazwa=="Zakończone"),
                            DataWyporzyczenia = new DateTime(2000, 11, 12),
                            DataZwrotu = new DateTime(2001, 11, 1)
                        },
                        new Models.Wyporzyczenie
                        {

                            Wyporzyczajacy = context.LibraryUser.ToList().Find(x => x.Nazwisko == "Krawczyk"),
                            WyporzyczanyZbior = context.Zbior.Include(x => x.Zasob).ToList().Find(x => x.CzyDostepny == false && x.Zasob.ISBN == "9780261102156"),
                            ObecnyStatus = context.Status.ToList().Find(x => x.Nazwa == "Trwa"),
                            DataWyporzyczenia = DateTime.Now,
                            DataZwrotu = DateTime.Now.AddDays(30),
                        },
                        new Models.Wyporzyczenie
                        {

                            Wyporzyczajacy = context.LibraryUser.ToList().Find(x => x.Nazwisko == "Mieścicka"),
                            WyporzyczanyZbior = context.Zbior.Include(x => x.Zasob).ToList().Find(x => x.CzyDostepny == false && x.Zasob.ISBN == "9788383223445"),
                            ObecnyStatus = context.Status.ToList().Find(x => x.Nazwa == "Trwa"),
                            DataWyporzyczenia = DateTime.Now,
                            DataZwrotu = DateTime.Now.AddDays(30),
                        },
                        new Models.Wyporzyczenie
                        {

                            Wyporzyczajacy = context.LibraryUser.ToList().Find(x => x.Nazwisko == "Mops"),
                            WyporzyczanyZbior = context.Zbior.Include(x => x.Zasob).ToList().Find(x => x.CzyDostepny == false && x.Zasob.ISBN == "9788383220147"),
                            ObecnyStatus = context.Status.ToList().Find(x => x.Nazwa == "Trwa"),
                            DataWyporzyczenia = DateTime.Now,
                            DataZwrotu = DateTime.Now.AddDays(30),
                        }
                    );
                    context.SaveChanges();
                }
                

            }
        }
    }
}
