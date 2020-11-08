using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;

namespace ProjectManagerInfra
{
    public class ProjectManagerInfraStack : Stack
    {
        internal ProjectManagerInfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Table ProjectIdeasDBProd = new Table(this, "ProjectIdeasDBProd", new TableProps
                                {
                                    PartitionKey = new Attribute { 
                                        Name = "Type", 
                                        Type = AttributeType.STRING 
                                    },
                                    SortKey = new Attribute { 
                                        Name = "Id", 
                                        Type = AttributeType.STRING 
                                    },
                                    BillingMode = BillingMode.PAY_PER_REQUEST,
                                    TableName = "ProjectIdeasDBProd"
                                });
        }
    }
}