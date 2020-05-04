﻿using System;
using System.Windows.Forms;

namespace JeremieLauncher
{
    public class Version
    {
        public Version(int versionMajor, int versionMinor, int versionPatch, int versionBuild)
        {
            VersionMajor = versionMajor;
            VersionMinor = versionMinor;
            VersionPatch = versionPatch;
            VersionBuild = versionBuild;
        }

        public bool IsSameVersion(Version other)
        {
            return VersionMajor == other.VersionMajor && VersionMinor == other.VersionMinor && VersionPatch == other.VersionPatch && VersionBuild == other.VersionBuild;
        }

        public override string ToString() => $"{VersionMajor}.{VersionMinor}.{VersionPatch} [build {VersionBuild}]";

        public static Version CreateFromString(string version)
        {
            int[] versionsI = new int[] {0,0,0,0 };
            string[] versions = version.Split('.');
            int length = versions.Length;
            if (length > 4)
                length = 4;
            if (!string.IsNullOrEmpty(version))
            {
                for (int i = 0; i < length; i++)
                {
                    versionsI[i] = int.Parse(versions[i]);
                }
            }
            return new Version(versionsI[0], versionsI[1], versionsI[2], versionsI[3]);
        }

        public static bool operator >(Version a, Version b)
        {
            bool result = false;
            if (a.VersionMajor > b.VersionMajor)
            {
                result = true;
            }
            else if (a.VersionMajor == b.VersionMajor && a.VersionMinor > b.VersionMinor)
            {
                result = true;
            }
            else if (a.VersionMajor == b.VersionMajor && a.VersionMinor == b.VersionMinor && a.VersionPatch > b.VersionPatch)
            {
                result = true;
            }
            else if (a.VersionMajor == b.VersionMajor && a.VersionMinor == b.VersionMinor && a.VersionPatch == b.VersionPatch && a.VersionBuild > b.VersionBuild)
            {
                result = true;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return (int) Math.Pow(VersionMajor.GetHashCode() + 1, Math.Pow(VersionMinor.GetHashCode() + 1, Math.Pow(VersionPatch.GetHashCode() + 1, VersionBuild.GetHashCode() + 1)));
        }


        public static bool operator <(Version a, Version b)
        {
            bool result = false;
            if (a.VersionMajor < b.VersionMajor)
            {
                result = true;
            }
            else if (a.VersionMajor == b.VersionMajor && a.VersionMinor < b.VersionMinor)
            {
                result = true;
            }
            else if (a.VersionMajor == b.VersionMajor && a.VersionMinor == b.VersionMinor && a.VersionPatch < b.VersionPatch)
            {
                result = true;
            }
            else if (a.VersionMajor == b.VersionMajor && a.VersionMinor == b.VersionMinor && a.VersionPatch == b.VersionPatch && a.VersionBuild < b.VersionBuild)
            {
                result = true;
            }
            return result;
        }

        public static bool operator >=(Version a, Version b)
        {
            return a > b || a == b;
        }

        public static bool operator <=(Version a, Version b)
        {
            return a < b || a == b;
        }

        public override bool Equals(object other)
        {
            bool result;
            if (!(other is Version))
            {
                result = false;
            }
            else
            {
                Version version = (Version)other;
                result = (this.version == version.version);
            }
            return result;
        }

        public static bool operator ==(Version a, Version b)
        {
            return a.version == b.version;
        }

        public static bool operator !=(Version a, Version b)
        {
            return !(a == b);
        }

        public int VersionMajor { get; private set; }
        public int VersionMinor { get; private set; }
        public int VersionPatch { get; private set; }
        public int VersionBuild { get; private set; }
        public string versionString { get { return $"{VersionMajor}.{VersionMinor}.{VersionPatch}.{VersionBuild}"; } }
        public int version { get { string temp = $"{VersionMajor}.{VersionMinor}.{VersionPatch}.{VersionBuild}"; return int.Parse(temp.Replace(".","")); } }
    }
}
