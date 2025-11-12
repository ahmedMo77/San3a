import { createSystem, defaultConfig, defineConfig } from "@chakra-ui/react"

export const buttonVariants = {
    primary: {
        bg: "primary.500",
        color: "white",
        _hover: { bg: "primary.600" }
    },
    secondary: {
        bg: "transparent",
        borderWidth: "1px",
        borderColor: "primary.500",
        color: "primary.500",
        _hover: {
            bg: "primary.600",
            color: "white"
        }
    }
}

export const FontVariants = {
    heading1: {
        color: "heading.500",
        fontWeight: "bold",
        fontSize: "7xl"
    },
    heading2:{
        color:"heading.500",
        fontWeight:"bold",
        fontSize:"5xl"
    },
    paragraph: {
        color: "heading.500",
        fontSize: "xl"
    },
    smallLabels: {
        color: "paragraph.500",
        fontSize: "md"
    },
}

const config = defineConfig({
    theme: {
        tokens: {
            colors: {
                primary: {
                    "500": { value: "#2563EB" },
                    "600": { value: "#0D2D72" }
                },
                accent: {
                    "500": { value: "#F59E0B" },
                    "600":{value:"#C3800E"}
                },
                heading: {
                    "500": { value: "#111827" }
                },
                paragraph: {
                    "500": { value: "#6B7280" }
                }
            },
        },
    },
})

export const system = createSystem(defaultConfig, config)