import {
  Checkbox,
  Flex,
  Grid,
  createListCollection,
  Link,
  Box,
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

const services = createListCollection({
  items: [
    { label: "سباكة", value: "سباكة" },
    { label: "نجارة", value: "نجارة" },
    { label: "كهرباء", value: "كهرباء" },
    { label: "دهانات", value: "دهانات" },
    { label: "اخرى", value: "اخرى" },
  ],
});

const RegisterWorkerForm = () => {
  const [selectedCity, setSelectedCity] = useState([]);
  const [selectedService, setSelectedService] = useState([]);
  const [formData, setFormData] = useState({
    name: "",
    contact: "",
    password: "",
    confirmPassword: "",
    nationalId: "",
    otherService: "",
  });
  const [isAgreed, setIsAgreed] = useState(false);
  const navigate = useNavigate();

  const handleInputChange = (name, value) => {
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const isFormValid = () => {
    const isServiceSelected = selectedService.length > 0;
    const isOtherServiceValid =
      selectedService[0] !== "اخرى" || formData.otherService.trim() !== "";
    const isCitySelected = selectedCity.length > 0;
    const areFieldsFilled =
      formData.name.trim() !== "" &&
      formData.contact.trim() !== "" &&
      formData.password.trim() !== "" &&
      formData.confirmPassword.trim() !== "" &&
      formData.nationalId.trim() !== "";

    return (
      isServiceSelected &&
      isOtherServiceValid &&
      isCitySelected &&
      areFieldsFilled &&
      isAgreed
    );
  };

  const handleSubmit = () => {
    if (isFormValid()) {
      navigate("إنشاء-حساب/تأكيد-الرمز-السري");
    }
  };

  return (
    <AuthBox stepper activeSteps={2}>
      <Flex direction={"column"} gap={6}>
        <Grid templateColumns="repeat(2, 1fr)" gap={4}>
          <FormSelect
            label="الحرفة"
            placeholder="اختر الحرفة"
            collection={services}
            value={selectedService}
            onChange={(d) => {
              setSelectedService(d.value);
            }}
          />
          {selectedService[0] === "اخرى" ? (
            <FormInput
              key={"others"}
              label={"اخرى"}
              placeholder={"اكتب اسم حرقتك"}
              value={formData.otherService}
              onChange={(e) =>
                handleInputChange("otherService", e.target.value)
              }
            />
          ) : (
            <Box />
          )}

          <FormSelect
            label="المحافظة"
            placeholder="اختر المحافظة"
            collection={cities}
            value={selectedCity}
            onChange={(d) => setSelectedCity(d.value)}
          />

          {inputFields.map((field) => (
            <FormInput
              key={field.name}
              label={field.label}
              placeholder={field.placeholder}
              type={field.type}
              value={formData[field.name]}
              onChange={(e) => handleInputChange(field.name, e.target.value)}
            />
          ))}
        </Grid>
        <Checkbox.Root
          size="sm"
          dir="rtl"
          checked={isAgreed}
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

export default RegisterWorkerForm;
