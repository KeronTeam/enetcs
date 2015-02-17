solution "enet-cs"
    configurations { "Release", "Debug" }
    location "build"
    
    filter "system:windows"
    	architecture "x32"

    project "libenet"
        kind "SharedLib"
        language "C"
        files { "enet/*.c" }
        includedirs { "enet/include/" }
        defines {
            "ENET_DLL", "HAS_FCNTL", "HAS_POLL",
            "HAS_GETHOSTBYNAME_R", "HAS_GETHOSTBYADDR_R",
            "HAS_INET_PTON", "HAS_INET_NTOP",
            "HAS_MSGHDR_FLAGS", "HAS_SOCKLEN_T"
        }

        filter "system:windows"
                targetname "ENet"
                targetsuffix "X86"
		links { "Winmm", "Ws2_32" }
        filter "system:linux"
                targetname "enet"
                targetextension ".so.1"

    project "ENet"
        kind "SharedLib"
        language "C#"
        framework "2.0"
        files { "ENetCS/**.cs" }
        flags { "Unsafe" }
        links { "System" }

    project "ENetDemo"
        kind "ConsoleApp"
        language "C#"
        framework "2.0"
        files { "ENetDemo/**.cs" }
        links { "ENet", "System" }
