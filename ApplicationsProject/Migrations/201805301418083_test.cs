namespace ApplicationsProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                        Content = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Destinations", t => t.DestinationId)
                .Index(t => t.ClientId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        DestinationId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(),
                        ContinentId = c.Int(),
                        Title = c.String(),
                        Content = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DestinationId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Continents", t => t.ContinentId)
                .Index(t => t.ClientId)
                .Index(t => t.ContinentId);
            
            CreateTable(
                "dbo.Continents",
                c => new
                    {
                        ContinentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContinentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "DestinationId", "dbo.Destinations");
            DropForeignKey("dbo.Destinations", "ContinentId", "dbo.Continents");
            DropForeignKey("dbo.Destinations", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Comments", "ClientId", "dbo.Clients");
            DropIndex("dbo.Destinations", new[] { "ContinentId" });
            DropIndex("dbo.Destinations", new[] { "ClientId" });
            DropIndex("dbo.Comments", new[] { "DestinationId" });
            DropIndex("dbo.Comments", new[] { "ClientId" });
            DropTable("dbo.Continents");
            DropTable("dbo.Destinations");
            DropTable("dbo.Comments");
            DropTable("dbo.Clients");
        }
    }
}
