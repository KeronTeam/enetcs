newoption {
    trigger = "arch",
    description = "Which architecture to build. Defaults to the underlying platform if not provided.",
    value = "ARCH",
    allowed = {
        { "x86", "32-bit" },
        { "x64", "64-bit" }
    }
}

if not _OPTIONS["arch"] then
    if os.is64bit() then
        _OPTIONS["arch"] = "x64"
    else
        _OPTIONS["arch"] = "x32"
    end
end

if _OPTIONS["arch"] == "x86" then
    _OPTIONS["arch"] = "x32"
end

solution "enet-cs"
    configurations { "Release", "Debug" }
    location "build"

    architecture (_OPTIONS["arch"])

    project "libenet"
        kind "SharedLib"
        language "C"
        targetdir "build/lib/%{cfg.buildcfg}-%{cfg.architecture}"
        files { "enet/*.c" }
        includedirs { "enet/include/" }
        defines {
            "ENET_DLL", "HAS_FCNTL", "HAS_POLL",
            "HAS_GETHOSTBYNAME_R", "HAS_GETHOSTBYADDR_R",
            "HAS_INET_PTON", "HAS_INET_NTOP",
            "HAS_MSGHDR_FLAGS", "HAS_SOCKLEN_T"
        }

        filter "configurations:Release"
            defines { "NDEBUG" }
            optimize "Full"

        filter "configurations:Debug"
            defines { "DEBUG" }
            flags { "Symbols" }

        filter "system:windows"
                links { "Winmm", "Ws2_32" }

    project "ENet"
        kind "SharedLib"
        language "C#"
        framework "2.0"
        targetdir "build/lib/%{cfg.buildcfg}-%{cfg.architecture}"
        files { "ENetCS/**.cs" }
        flags { "Unsafe" }
        links { "System" }

    project "ENetDemo"
        kind "ConsoleApp"
        language "C#"
        framework "2.0"
        targetdir "build/bin/%{cfg.buildcfg}-%{cfg.architecture}"
        files { "ENetDemo/**.cs" }
        links { "ENet", "System" }
