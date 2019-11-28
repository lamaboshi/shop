namespace WpfApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "Totile", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "Totile");
        }
    }
}
