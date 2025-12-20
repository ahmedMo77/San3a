import { Text } from "@chakra-ui/react"
import {Link} from "react-router-dom"

function Logo() {
    return (
        <Text fontWeight={"bold"} fontSize={"3xl"}>
            <Link to={"/"}>
            <Text as="span" color={"primary.500"}>صن</Text>
            <Text as="span" color={"accent.500"}>عة</Text>
            </Link>
        </Text>
    )
}
export default Logo