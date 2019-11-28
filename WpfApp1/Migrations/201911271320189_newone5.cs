namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newone5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BillStores", "BillId", "dbo.Bills");
            DropForeignKey("dbo.BillStores", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Stores", "Materail_Id", "dbo.Materails");
            DropIndex("dbo.BillStores", new[] { "StoreId" });
            DropIndex("dbo.BillStores", new[] { "BillId" });
            DropIndex("dbo.Stores", new[] { "Materail_Id" });
            CreateTable(
                "dbo.BillMaterails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        MaterailId = c.Int(nullable: false),
                        BillId = c.Int(nullable: false),
                        Isdelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.BillId, cascadeDelete: true)
                .ForeignKey("dbo.Materails", t => t.MaterailId, cascadeDelete: true)
                .Index(t => t.MaterailId)
                .Index(t => t.BillId);
            
            DropTable("dbo.BillStores");
            DropTable("dbo.Stores");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.BillMaterails", "MaterailId", "dbo.Materails");
            DropForeignKey("dbo.BillMaterails", "BillId", "dbo.Bills");
            DropIndex("dbo.BillMaterails", new[] { "BillId" });
            DropIndex("dbo.BillMaterails", new[] { "MaterailId" });
            DropTable("dbo.BillMaterails");
            CreateIndex("dbo.Stores", "Materail_Id");
            CreateIndex("dbo.BillStores", "BillId");
            CreateIndex("dbo.BillStores", "StoreId");
            AddForeignKey("dbo.Stores", "Materail_Id", "dbo.Materails", "Id");
            AddForeignKey("dbo.BillStores", "StoreId", "dbo.Stores", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BillStores", "BillId", "dbo.Bills", "Id", cascadeDelete: true);
        }
    }
}
