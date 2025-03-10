// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

namespace Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;

public enum TellState
{
    Active,
    Waiting,
    Paused,
    Error,
    Complete,
    Removed,
    Stopped
}
