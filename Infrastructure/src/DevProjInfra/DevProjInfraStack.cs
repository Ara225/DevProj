using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;

namespace DevProjInfra
{
    public class DevProjInfraStack : Stack
    {
        internal DevProjInfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Table ProjectIdeasDBProd = new Table(this, "DevProjUsersTable", new TableProps
            {
                PartitionKey = new Attribute
                {
                    Name = "Id",
                    Type = AttributeType.STRING
                },
                BillingMode = BillingMode.PAY_PER_REQUEST,
                TableName = "DevProjUsersTable"
            });
        }
    }
}