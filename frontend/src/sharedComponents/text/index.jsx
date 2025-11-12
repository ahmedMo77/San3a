import { Text } from "@chakra-ui/react"
import { FontVariants } from "../../theme"

export function HeadingText({ children, value }) {
    return (
        <Text textAlign={"center"} {...FontVariants.heading2}>{children ?? value}</Text>
    )
}

export function SubText({ children, value,...props }) {
    return (
        <Text {...FontVariants.paragraph} {...props}>{children ?? value}</Text>
    )
}

export function PrimaryText(){
    return(
        <Text as={"span"} color={"primary.500"}> صنّعة </Text>
    )
}