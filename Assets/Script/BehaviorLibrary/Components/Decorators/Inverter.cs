using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace BehaviorLibrary.Components.Decorators
{
    public class Inverter : BehaviorComponent
    {

        private BehaviorComponent _Behavior;

        /// <summary>
        /// inverts the given behavior
        /// -Returns Success on Failure or Error
        /// -Returns Failure on Success 
        /// -Returns Running on Running
        /// /*    按照节点返回结果的反义来返回,达到NOT的效果？    */
        /// /*    -返回Success如果节点返回Failure或者出错    */
        /// /*    -返回Failure如果节点返回Success    */
        /// /*    -返回Running如果节点返回Running    */
        /// </summary>
        /// <param name="behavior"></param>
        public Inverter(BehaviorComponent behavior) 
        {
            _Behavior = behavior;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            try
            {
                switch (_Behavior.Behave())
                {
                    case BehaviorReturnCode.Failure:
                        ReturnCode = BehaviorReturnCode.Success;
                        return ReturnCode;
                    case BehaviorReturnCode.Success:
                        ReturnCode = BehaviorReturnCode.Failure;
                        return ReturnCode;
                    case BehaviorReturnCode.Running:
                        ReturnCode = BehaviorReturnCode.Running;
                        return ReturnCode;
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.Error.WriteLine(e.ToString());
#endif
                ReturnCode = BehaviorReturnCode.Success;
                return ReturnCode;
            }

            ReturnCode = BehaviorReturnCode.Success;
            return ReturnCode;

        }

    }
}
