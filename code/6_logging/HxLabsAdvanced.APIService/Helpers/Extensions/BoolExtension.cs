using System;

namespace HxLabsAdvanced.APIService.Helpers.Extensions
{
    public static class BoolExtension
    {
        public static TResult Is<TResult>(this bool condition, Func<TResult> trueAction, Func<TResult> falseAction)
        {
            if (condition)
            {
                return trueAction.Invoke();
            }

            return falseAction.Invoke();
        }

        public static void Is(this bool condition, Action trueAction, Action falseAction)
        {
            if (condition)
            {
                trueAction.Invoke();

                return;
            }

            falseAction.Invoke();
        }
    }
}
