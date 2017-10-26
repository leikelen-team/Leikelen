using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace cl.uv.leikelen.API.Module.Input
{
    /// <summary>
    /// Interface for send images to the integrated video player
    /// </summary>
    public interface IVideo
    {
        /// <summary>
        /// Occurs when [color image arrived].
        /// </summary>
        event EventHandler<ImageSource> ColorImageArrived;
        /// <summary>
        /// Occurs when [skeleton image arrived].
        /// </summary>
        event EventHandler<ImageSource> SkeletonImageArrived;

        /// <summary>
        /// Determines whether [is color enabled].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is color enabled]; otherwise, <c>false</c>.
        /// </returns>
        bool IsColorEnabled();
        /// <summary>
        /// Determines whether [is skeleton enabled].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is skeleton enabled]; otherwise, <c>false</c>.
        /// </returns>
        bool IsSkeletonEnabled();
        /// <summary>
        /// Enables the color layer.
        /// </summary>
        void EnableColor();
        /// <summary>
        /// Enables the skeleton layer.
        /// </summary>
        void EnableSkeleton();
        /// <summary>
        /// Disables the color layer.
        /// </summary>
        void DisableColor();
        /// <summary>
        /// Disables the skeleton layer.
        /// </summary>
        void DisableSkeleton();
    }
}
