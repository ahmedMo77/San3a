import { Button as CButton, Flex } from "@chakra-ui/react"
import { buttonVariants } from "../../theme"



function Button({ children, value, variant = "primary", onClick, icon, ...props }) {
    return (
        <CButton
            {...buttonVariants[variant]}
            onClick={onClick}
            {...props}
            p={6}
        >
            <Flex gap={2} alignItems="center">
            {icon}
            {value}
            </Flex>
        </CButton>
    )
}

function ButtonContainer({ primaryProps,icon,primaryText = "تسجيل دخول", secondaryText = "حساب جديد", onPrimaryClick, onSecondaryClick, gap = 4 }) {
    return (
        <Flex gap={gap} >
            <Button value={primaryText} onClick={onPrimaryClick} icon={icon} {...primaryProps} />
            <Button value={secondaryText} variant="secondary" onClick={onSecondaryClick} icon={icon} />
        </Flex>
    )
}

export { Button }
export default ButtonContainer