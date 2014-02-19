using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace BehaviorLibrary.Components.Composites
{
    public class Selector : BehaviorComponent
    {

        protected BehaviorComponent[] _Behaviors;


        /// <summary>
        /// Selects among the given behavior components
        /// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
        /// -Returns Success if a behavior component returns Success
        /// -Returns Running if a behavior component returns Running
        /// -Returns Failure if all behavior components returned Failure
        /// /*    在给定行为中选择一个行为节点(会遍历每个节点)    */
        /// /*    变现类似OR的效果，并且允许发生错误，直到返回Success或者全都Failure    */
        /// /*    -返回Success如果有一个返回Success    */
        /// /*    -返回Running如果有一个返回Running    */
        /// /*    -返回Failure如果所有的都返回Failure    */
        /// </summary>
        /// <param name="behaviors">one to many behavior components</param>
        public Selector(params BehaviorComponent[] behaviors)
        {
            _Behaviors = behaviors;
        }

        /// <summary>
        /// performs the given behavior
        /// </summary>
        /// <returns>the behaviors return code</returns>
        public override BehaviorReturnCode Behave()
        {
            
            for (int i = 0; i < _Behaviors.Length; i++)
            {
                try
                {
                    switch (_Behaviors[i].Behave())
                    {
                        case BehaviorReturnCode.Failure:
                            continue;
                        case BehaviorReturnCode.Success:
                            ReturnCode = BehaviorReturnCode.Success;
                            return ReturnCode;
                        case BehaviorReturnCode.Running:
                            ReturnCode = BehaviorReturnCode.Running;
                            return ReturnCode;
                        default:
                            continue;
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                Console.Error.WriteLine(e.ToString());
#endif
                    continue;
                }
            }

            ReturnCode = BehaviorReturnCode.Failure;
            return ReturnCode;
        }
    }
}
