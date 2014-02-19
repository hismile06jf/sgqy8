using System;
using BehaviorLibrary.Components;

namespace BehaviorLibrary
{
	public class StatefulSelector : BehaviorComponent
	{
		private BehaviorComponent[] _Behaviors;

		private int _LastBehavior = 0;

		/// <summary>
		/// Selects among the given behavior components (stateful on running) 
		/// Performs an OR-Like behavior and will "fail-over" to each successive component until Success is reached or Failure is certain
		/// -Returns Success if a behavior component returns Success
		/// -Returns Running if a behavior component returns Running
		/// -Returns Failure if all behavior components returned Failure
        /// /*    在给定的行为节点中选择一个（会遍历所有节点，失败后会继续下一个，成功后会重新遍历）    */
        /// /*    表现一个类似OR的效果，并且会允许发生错误，知道有一个行为节点返回成功或者全失败    */
        /// /*    -返回Success，如果有一个节点返回Success    */
        /// /*    -返回Running，如果有一个节点返回Running    */
        /// /*    -返回Failure，如果所有节点都返回Failure    */
		/// </summary>
		/// <param name="behaviors">one to many behavior components</param>
		public StatefulSelector(params BehaviorComponent[] behaviors){
			this._Behaviors = behaviors;
		}

		/// <summary>
		/// performs the given behavior
		/// </summary>
		/// <returns>the behaviors return code</returns>
		public override BehaviorReturnCode Behave(){

			for(; _LastBehavior < _Behaviors.Length; _LastBehavior++){
				try{
					switch (_Behaviors[_LastBehavior].Behave()){
					case BehaviorReturnCode.Failure:
						continue;
					case BehaviorReturnCode.Success:
						_LastBehavior = 0;
						ReturnCode = BehaviorReturnCode.Success;
						return ReturnCode;
					case BehaviorReturnCode.Running:
						ReturnCode = BehaviorReturnCode.Running;
						return ReturnCode;
					default:
						continue;
					}
				}
				catch (Exception e){
#if DEBUG
					Console.Error.WriteLine(e.ToString());
#endif
					continue;
				}
			}

			_LastBehavior = 0;
			ReturnCode = BehaviorReturnCode.Failure;
			return ReturnCode;
		}
	}
}
