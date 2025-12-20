import {
  Checkbox,
  Flex,
  Grid,
  createListCollection,
  Link,
} from "@chakra-ui/react";
import { useState } from "react";
import { FormInput, FormSelect } from "../../../../sharedComponents/Forms";
import AuthBox from "../../authBox";
import ButtonContainer from "../../../../sharedComponents/Button";
import { Link as RouterLink, useNavigate } from "react-router-dom";

const inputFields = [
  { name: "name", label: "الاسم", placeholder: "اسمك بالكامل" },
  {
    name: "contact",
    label: "البريد الإلكتروني / رقم التليفون",
    placeholder: "البريد الالكتروني/ رقم التليفون",
  },
  {
    name: "password",
    label: "كلمة السر",
    placeholder: "كلمة السر",
    type: "password",
  },
  {
    name: "confirmPassword",
    label: "تأكيد كلمة السر",
    placeholder: "تأكيد كلمة السر",
    type: "password",
  },
  {
    name: "nationalId",
    label: "الرقم القومي",
    placeholder: "0 0000 0000 00000",
  },
];
const cities = createListCollection({
  items: [
    { label: "القاهرة", value: "القاهرة" },
    { label: "الإسكندرية", value: "الإسكندرية" },
    { label: "المنوفية", value: "المنوفية" },
    { label: "الغربية", value: "الغربية" },
    { label: "الإسماعيلية", value: "الإسماعيلية" },
    { label: "الدقهلية", value: "الدقهلية" },
    { label: "القليوبية", value: "القليوبية" },
  ],
});

const RegisterClientForm = () => {
  const [selectedCity, setSelectedCity] = useState([]);
  const [isAgreed, setIsAgreed] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    contact: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const navigate = useNavigate();

  const handleInputChange = (name, value) => {
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = () => {
    navigate("/إنشاء-حساب/تأكيد-الرمز-السري");
  };

  const isFormValid = () => {
    const isCitySelected = selectedCity.length > 0;
    const areFieldsFilled =
      formData.name.trim() !== "" &&
      formData.contact.trim() !== "" &&
      formData.email.trim() !== "" &&
      formData.password.trim() !== "" &&
      formData.confirmPassword.trim() !== "";
    return isCitySelected, areFieldsFilled, isAgreed;
  };

  return (
    <AuthBox stepper activeSteps={2}>
      <Flex gap={6} direction="column" height="full">
        <Grid templateColumns="repeat(2, 1fr)" gap={4}>
          {inputFields.map((field) => (
            <FormInput
              key={field.name}
              label={field.label}
              placeholder={field.placeholder}
              type={field.type}
              onChange={(e) => handleInputChange(field.name, e.target.value)}
            />
          ))}
          <FormSelect
            label="المحافظة"
            placeholder="اختر المحافظة"
            collection={cities}
            value={selectedCity}
            onChange={(d) => setSelectedCity(d.value)}
          />
        </Grid>
        <Checkbox.Root
          checked={isAgreed}
          size="sm"
          dir="rtl"
          onCheckedChange={(e) => setIsAgreed(e.checked)}
        >
          <Checkbox.HiddenInput />
          <Checkbox.Control variant="primary">
            <Checkbox.Indicator />
          </Checkbox.Control>
          <Checkbox.Label fontWeight={"light"}>
            <Link
              _focus={{ textDecoration: "none", outline: "none" }}
              as={RouterLink}
              to={""}
              textDcoration="underline"
            >
              الشروط و الأحكام
            </Link>
            موافق علي
          </Checkbox.Label>
        </Checkbox.Root>
        <ButtonContainer
          disabled={!isFormValid()}
          direction="column"
          primaryText="تأكيد"
          onPrimaryClick={handleSubmit}
        />
      </Flex>
    </AuthBox>
  );
};

export default RegisterClientForm;
