using System;

namespace Unity.Build
{
    /// <summary>
    /// Holds contextual information when cleaning a build.
    /// </summary>
    public sealed class CleanContext : ContextBase
    {
        /// <summary>
        /// Get a clean result representing a success.
        /// </summary>
        /// <returns>A new clean result instance.</returns>
        public CleanResult Success() => CleanResult.Success(BuildPipeline, BuildConfiguration);

        /// <summary>
        /// Get a clean result representing a failure.
        /// </summary>
        /// <param name="reason">The reason of the failure.</param>
        /// <returns>A new clean result instance.</returns>
        public CleanResult Failure(string reason) => CleanResult.Failure(BuildPipeline, BuildConfiguration, reason);

        /// <summary>
        /// Get a clean result representing a failure.
        /// </summary>
        /// <param name="exception">The exception that was thrown.</param>
        /// <returns>A new clean result instance.</returns>
        public CleanResult Failure(Exception exception) => CleanResult.Failure(BuildPipeline, BuildConfiguration, exception);

        /// <summary>
        /// Get the build result of the last <see cref="BuildConfiguration.Build"/> performed.
        /// </summary>
        /// <returns>The build result if found, <see langword="null"/> otherwise.</returns>
        public BuildResult GetBuildResult() => BuildConfiguration.GetBuildResult();

        internal CleanContext(BuildPipelineBase pipeline, BuildConfiguration config)
            : base(pipeline, config)
        {
        }
    }
}
