namespace ManageSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBacteriaDetailedData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BacteriaDetailedData",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        V0002 = c.String(maxLength: 1000),
                        V0004 = c.String(maxLength: 1000),
                        V0008 = c.String(maxLength: 1000),
                        V0016 = c.String(maxLength: 1000),
                        V0032 = c.String(maxLength: 1000),
                        V0064 = c.String(maxLength: 1000),
                        V0125 = c.String(maxLength: 1000),
                        V0025 = c.String(maxLength: 1000),
                        V0005 = c.String(maxLength: 1000),
                        V1001 = c.String(maxLength: 1000),
                        V1002 = c.String(maxLength: 1000),
                        V1004 = c.String(maxLength: 1000),
                        V1008 = c.String(maxLength: 1000),
                        V1016 = c.String(maxLength: 1000),
                        V1032 = c.String(maxLength: 1000),
                        V1064 = c.String(maxLength: 1000),
                        V1128 = c.String(maxLength: 1000),
                        V1256 = c.String(maxLength: 1000),
                        V1512 = c.String(maxLength: 1000),
                        Ecoff = c.String(maxLength: 1000),
                        Distributions = c.String(maxLength: 1000),
                        Observations = c.String(maxLength: 1000),
                        OrganismTypeId = c.Long(nullable: false),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        DeleteTime = c.DateTime(nullable: false),
                        Version = c.Long(nullable: false),
                        Mark = c.Int(nullable: false),
                        Describe = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BacteriaDetailedData");
        }
    }
}
