using Dalamud.Game;
using Dalamud.Game.Internal;
using System;
using System.Runtime.InteropServices;

namespace MOAction
{
    public class MOActionAddressResolver : BaseAddressResolver
    {

        public IntPtr RequestAction { get; private set; }

        public IntPtr SetUiMouseoverEntityId { get; private set; }

        public IntPtr ResolvePlaceholderText { get; private set; }
        public IntPtr RequestActionLocation { get; private set; }

        public IntPtr PostRequest { get; private set; }

        public IntPtr FieldMO;
        public IntPtr FocusTarg;
        public IntPtr RegularTarg;
        public IntPtr PronounModule;
        public IntPtr GetGroupTimer;

        public IntPtr AnimLock;
        // This is so hacky. One day I'm going to figure out how to get proper sigs.
        protected override void Setup64Bit(SigScanner sig)
        {
            RequestAction = sig.ScanText("E8 ?? ?? ?? ?? EB 64 B1 01");

            SetUiMouseoverEntityId = sig.ScanText("E8 ?? ?? ?? ?? 48 8B 6C 24 ?? 48 8B 5C 24 ?? 4C 8B 7C 24 ?? 41 83 FC 02");

            ResolvePlaceholderText = sig.ScanText("E8 ?? ?? ?? ?? 48 8B 5C 24 ?? EB 0C");

            RequestActionLocation = sig.ScanText("E8 ?? ?? ?? ?? 3C 01 0F 85 ?? ?? ?? ?? EB 46");

            PostRequest = sig.ScanText("E8 ?? ?? ?? ?? 40 0F B6 C6 4C 8B AC 24 ?? ?? ?? ??");
            FieldMO = sig.GetStaticAddressFromSig("E8 ?? ?? ?? ?? 83 BF ?? ?? ?? ?? ?? 0F 84 ?? ?? ?? ?? 48 8D 4C 24 ??", 0x283);
            FocusTarg = sig.GetStaticAddressFromSig("48 8B 0D ?? ?? ?? ?? 48 89 5C 24 ?? BB ?? ?? ?? ?? 48 89 7C 24 ??", 0);
            RegularTarg = sig.GetStaticAddressFromSig("F3 0F 11 05 ?? ?? ?? ?? EB 27", 0) + 0x4;
            PronounModule = sig.GetStaticAddressFromSig("48 8B 0D ?? ?? ?? ?? 48 8D 05 ?? ?? ?? ?? 48 89 05 ?? ?? ?? ?? 48 85 C9 74 0C", 0);
            AnimLock = sig.GetStaticAddressFromSig("E8 ?? ?? ?? ?? 84 DB 75 37", 0x1D);
            GetGroupTimer = sig.ScanText("E8 ?? ?? ?? ?? 0F 57 FF 48 85 C0");
        }

        [StructLayout(LayoutKind.Explicit, Size = 0x14)]
        public unsafe struct RecastTimer
        {
            [FieldOffset(0x0)] public byte IsActive;
            [FieldOffset(0x4)] public uint ActionID;
            [FieldOffset(0x8)] public float Elapsed;
            [FieldOffset(0xC)] public float Total;
        }
    
    }
}
