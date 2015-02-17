solution "enet-cs"
    configurations { "Release", "Debug" }
    location "build"

    filter { "system:windows" }
        architecture "universal"

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
