namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Name = c.String(),
                        DateOut = c.DateTime(nullable: false),
                        Isdelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillStores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                        BillId = c.Int(nullable: false),
                        Isdelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.BillId);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        DateIn = c.DateTime(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        Isdelete = c.Boolean(nullable: false),
                        Materail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Materails", t => t.Materail_Id)
                .Index(t => t.Materail_Id);
            
            CreateTable(
                "dbo.Materails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TypeId = c.Int(nullable: false),
                        Isdelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Isdelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Materails", "TypeId", "dbo.Types");
            DropForeignKey("dbo.Stores", "Materail_Id", "dbo.Materails");
            DropForeignKey("dbo.BillStores", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.BillStores", "BillId", "dbo.Bills");
            DropIndex("dbo.Materails", new[] { "TypeId" });
            DropIndex("dbo.Stores", new[] { "Materail_Id" });
            DropIndex("dbo.BillStores", new[] { "BillId" });
            DropIndex("dbo.BillStores", new[] { "StoreId" });
            DropTable("dbo.Types");
            DropTable("dbo.Materails");
            DropTable("dbo.Stores");
            DropTable("dbo.BillStores");
            DropTable("dbo.Bills");
        }
    }
}
