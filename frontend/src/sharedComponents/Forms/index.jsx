import { Field, Input, Portal, Select } from "@chakra-ui/react";

export const FormInput = ({ label, placeholder, type = "text", ...props }) => (
  <Field.Root required>
    <Field.Label fontWeight="semibold">
      {label}
      <Field.RequiredIndicator />
    </Field.Label>
    <Input
      type={type}
      placeholder={placeholder}
      _placeholder={{ color: "paragraph.500" }}
      {...props}
    />
    <Field.ErrorText>هذا الحقل مطلوب</Field.ErrorText>
  </Field.Root>
);

export const FormSelect = ({
  label,
  placeholder,
  collection,
  value,
  onChange,
}) => (
  <Field.Root required>
    <Select.Root
      dir="rtl"
      collection={collection}
      value={value}
      onValueChange={onChange}
    >
      <Select.Label>{label}</Select.Label>
      <Select.Control>
        <Select.Trigger>
          <Select.ValueText placeholder={placeholder} />
        </Select.Trigger>
        <Select.IndicatorGroup>
          <Select.Indicator />
        </Select.IndicatorGroup>
      </Select.Control>

      <Portal>
        <Select.Positioner>
          <Select.Content>
            {collection.items.map((item) => (
              <Select.Item key={item.value} item={item}>
                {item.label}
                <Select.ItemIndicator />
              </Select.Item>
            ))}
          </Select.Content>
        </Select.Positioner>
      </Portal>
    </Select.Root>
  </Field.Root>
);
