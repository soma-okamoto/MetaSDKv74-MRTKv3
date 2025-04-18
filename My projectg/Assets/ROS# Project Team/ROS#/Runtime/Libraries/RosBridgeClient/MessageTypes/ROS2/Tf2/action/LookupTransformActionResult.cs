/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

#if ROS2
using RosSharp.RosBridgeClient.MessageTypes.Std;
namespace RosSharp.RosBridgeClient.MessageTypes.Tf2
{
    public class LookupTransformActionResult : ActionResult<LookupTransformResult>
    {
        public const string RosMessageName = "tf2_msgs/action/LookupTransformActionResult";

        public LookupTransformActionResult() : base()
        {
            this.values = new LookupTransformResult();
        }

        public LookupTransformActionResult(Header header, string action, sbyte status, bool result, string id, LookupTransformResult values) : base(header, action, status, result, id)
        {
            this.values = values;
        }
    }
}
#endif
