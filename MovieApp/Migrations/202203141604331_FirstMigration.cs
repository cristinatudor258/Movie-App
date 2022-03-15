namespace MovieApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "User", c => c.String());
            AddColumn("dbo.Ratings", "MovieID", c => c.Int(nullable: false));
            AddColumn("dbo.Ratings", "MovieTitle", c => c.String());
            DropColumn("dbo.Ratings", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "UserID", c => c.Int(nullable: false));
            DropColumn("dbo.Ratings", "MovieTitle");
            DropColumn("dbo.Ratings", "MovieID");
            DropColumn("dbo.Ratings", "User");
        }
    }
}
