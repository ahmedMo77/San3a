import AuthLayout from "../../../components/auth/authLayout";
import RegisterTypeSelector from "../../../components/auth/forms/signup/registerTypeSelector";

const SignUpPage = () => {
  return (
    <AuthLayout>
      <RegisterTypeSelector />
    </AuthLayout>
  );
};

export default SignUpPage;
