﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Tests;
using Microsoft.Data.Entity;

namespace GraphQL.Relay.StarWars.DB
{
    public class StarWarsDbContext : DbContext
    {
        public StarWarsDbContext()
        {
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );
            //optionsBuilder.UseSqlServer( "Server=.;Database=GraphQL;Integrated Security=SSPI;MultipleActiveResultSets=True;" );
        }

        internal void InitializeData()
        {
            Humans.Add( new Human
            {
                Id = "1",
                Name = "Luke",
                HomePlanet = "Tatooine"
            } );
            Humans.Add( new Human
            {
                Id = "2",
                Name = "Vader",
                HomePlanet = "Tatooine"
            } );

            Droids.Add( new Droid
            {
                Id = "3",
                Name = "R2-D2",
                PrimaryFunction = "Astromech"
            } );
            Droids.Add( new Droid
            {
                Id = "4",
                Name = "C-3PO",
                PrimaryFunction = "Protocol"
            } );

            var ep4 = new StarWarsEpisod
            {
                Id = (int)StarWarsEpisodEnum.NEWHOPE,
                Name = "NEWHOPE"
            };
            var ep5 = new StarWarsEpisod
            {
                Id = (int)StarWarsEpisodEnum.EMPIRE,
                Name = "EMPIRE"
            };
            var ep6 = new StarWarsEpisod
            {
                Id = (int)StarWarsEpisodEnum.JEDI,
                Name = "JEDI"
            };
            Episods.Add( ep4 );
            Episods.Add( ep5 );
            Episods.Add( ep6 );

            Factions.Add( new Faction
            {
                FactionId = 1,
                FactionName = "Alliance to Restore the Republic"
            } );
            SaveChanges();
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.Entity<StarWarsCharacter>().ToTable( "Character" );
            modelBuilder.Entity<Human>().ToTable( "Human" );
            modelBuilder.Entity<Droid>().ToTable( "Droid" );
            modelBuilder.Entity<StarWarsEpisod>().Property( e => e.Id ); //.HasDatabaseGeneratedOption( DatabaseGeneratedOption.None );
            //modelBuilder.Entity<CharactersAppearance>().ToTable( "Appearance" );
            //modelBuilder.Entity<FriendShip>().HasKey( c => new { c.CharacterId, c.FriendId } );
            //modelBuilder.Entity<StarWarsCharacter>()
            //   .HasMany( c => c.Friends )
            //     .WithMany()
            //        .Map( m =>
            //        {
            //            m.MapLeftKey( "CharacterId" );
            //            m.MapRightKey( "FriendId" );
            //            m.ToTable( "Friends" );
            //        } );
            //modelBuilder.Entity<StarWarsCharacter>()
            //    .HasMany( c => c.Appearances )
            //     .WithMany()
            //        .Map( m =>
            //        {
            //            m.MapLeftKey( "CharacterId" );
            //            m.MapRightKey( "EpisodId" );
            //            m.ToTable( "CharactersAppearance" );
            //        } );

        }

        public DbSet<Human> Humans { get; set; }
        public DbSet<Droid> Droids { get; set; }

        public DbSet<StarWarsEpisod> Episods { get; set; }

        public DbSet<Faction> Factions { get; set; }
        public DbSet<Ship> Ships { get; set; }
    }

    public class CharactersAppearance
    {
        public int CharacterId { get; set; }

        public StarWarsEpisod Episod { get; set; }
    }

    public enum StarWarsEpisodEnum
    {
        NEWHOPE = 4,
        EMPIRE = 5,
        JEDI = 6
    }

    public class StarWarsEpisod
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
