import { Menu } from "@chakra-ui/react"
import React from "react"

const DropdownMenu = ({ trigger, items = [], onSelect, ...props }) => {
    return (
        <Menu.Root {...props}>
            <Menu.Trigger asChild>
                {trigger}
            </Menu.Trigger>
            <Menu.Positioner>
                <Menu.Content maxH="140px" overflowY="auto">
                    {items.map((item, index) => {
                        const label = typeof item === 'object' ? item.label : item;
                        const value = typeof item === 'object' ? item.value : item;
                        return (
                            <Menu.Item
                                key={index}
                                value={value}
                                onClick={() => onSelect && onSelect(value)}
                                cursor="pointer"
                            >
                                {label}
                            </Menu.Item>
                        )
                    })}
                </Menu.Content>
            </Menu.Positioner>
        </Menu.Root>
    )
}

export default DropdownMenu
