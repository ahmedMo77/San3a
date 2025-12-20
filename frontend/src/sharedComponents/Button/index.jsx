import { Button as CButton, Flex } from "@chakra-ui/react";
import { buttonVariants } from "../../theme";
import DropdownMenu from "../dropdownMenu";

function Button({
  children,
  value,
  variant = "primary",
  onClick,
  icon,
  ...props
}) {
  return (
    <CButton {...buttonVariants[variant]} onClick={onClick} {...props} p={6}>
      <Flex gap={2} alignItems="center">
        {icon}
        {value}
      </Flex>
    </CButton>
  );
}

function ButtonContainer({
  primaryText,
  secondaryText,
  primaryIcon,
  secondaryIcon,
  onPrimaryClick,
  onSecondaryClick,
  gap = 4,
  primaryItems,
  secondaryItems,
  onPrimarySelect,
  onSecondarySelect,
  ...props
}) {
  const hasPrimary =
    secondaryText || (secondaryItems && secondaryItems.length > 0);
  const renderPrimary = () => {
    const btn = (
      <Button
        value={primaryText}
        onClick={onPrimaryClick}
        icon={primaryIcon}
        disabled={props.disabled}
      />
    );
    if (primaryItems && primaryItems.length > 0) {
      return (
        <DropdownMenu
          trigger={btn}
          items={primaryItems}
          onSelect={onPrimarySelect}
        />
      );
    }
    return btn;
  };
  const renderSecondary = () => {
    const btn = (
      <Button
        value={secondaryText}
        onClick={onSecondaryClick}
        icon={secondaryIcon}
        variant="secondary"
      />
    );
    if (secondaryItems && secondaryItems.length > 0) {
      return (
        <DropdownMenu
          trigger={btn}
          items={secondaryItems}
          onSelect={onSecondarySelect}
        />
      );
    }
    return btn;
  };
  return (
    <Flex gap={gap} {...props}>
      {hasPrimary && renderSecondary()}
      {renderPrimary()}
    </Flex>
  );
}

export { Button };
export default ButtonContainer;
