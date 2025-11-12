import { Icon, Flex, Text } from "@chakra-ui/react"
import { FontVariants } from "../../theme"

function IconContainer({ icon, sub, title, textWidth, iconSize, subTextWidth, rounded = "xl", width = "14", height = "14", fontWeight, backgroundColor, fontSize, ...props }) {
    return (
        <Flex direction={"column"} alignItems={"center"} rounded={rounded} gap={4}>
            <Flex alignItems={"center"} justifyContent={"center"} backgroundColor={backgroundColor} width={width} height={height} rounded={rounded}>
                <Icon
                    as={icon.type}
                    w={iconSize}
                    h={iconSize}
                />
            </Flex>
            <Text width={textWidth} textAlign={"center"} fontWeight={fontWeight} fontSize={fontSize} {...props}>{title}</Text>
            <Text width={subTextWidth} textAlign={"center"} {...FontVariants.smallLabels}>{sub}</Text>
        </Flex>
    )
}

export default IconContainer