using System;
using BehaviorLibrary.Components;

namespace BehaviorLibrary
{
	public class StatefulSequence : BehaviorComponent
	{
		private BehaviorComponent[] _Behaviors;

		private int _LastBehavior = 0;

		/// <summary>
		/// attempts to run the behaviors all in one cycle (stateful on running)
		/// -Returns Success when all are successful
		/// -Returns Failure if one behavior fails or an error occurs
		/// -Does not Return Running
        /// /*    尝试在一个运行所有的行为节点，类似and的序列（会遍历所有节点，失败后会重新遍历,成功则按照上次最后一个继续，所有成功后会重新遍历）    */
        /// /*    -当所有执行结果成功时返回Success    */
        /// /*    -有一个执行结果为Failure或者出错就返回Failure    */
        /// /*    -不返回Running状态    */
		/// </summary>
		/// <param name="behaviors"></param>
		public StatefulSequence (params BehaviorComponent[] behaviors){
			this._Behaviors = behaviors;
		}

		/// <summary>
		/// performs the given behavior
		/// </summary>
		/// <returns>the behaviors return code</returns>
		public override BehaviorReturnCode Behave(){

			//start from last remembered position
			for(; _LastBehavior < _Behaviors.Length;_LastBehavior++){
				try{
					switch (_Behaviors[_LastBehavior].Behave()){
					case BehaviorReturnCode.Failure:
						_LastBehavior = 0;
						ReturnCode = BehaviorReturnCode.Failure;
						return ReturnCode;
					case BehaviorReturnCode.Success:
						continue;
					case BehaviorReturnCode.Running:
						ReturnCode = BehaviorReturnCode.Running;
						return ReturnCode;
					default:
						_LastBehavior = 0;
						ReturnCode = BehaviorReturnCode.Success;
						return ReturnCode;
					}
				}
				catch (Exception e){
#if DEBUG
					Console.Error.WriteLine(e.ToString());
#endif
					_LastBehavior = 0;
					ReturnCode = BehaviorReturnCode.Failure;
					return ReturnCode;
				}
			}

			_LastBehavior = 0;
			ReturnCode = BehaviorReturnCode.Success;
			return ReturnCode;
		}


	}
}

