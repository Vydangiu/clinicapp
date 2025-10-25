class ApiConfig {
  // ✅ Đổi true/false tuỳ bạn đang chạy trên điện thoại thật hay emulator
  static const bool runOnRealPhone =
      true; // true = điện thoại thật, false = emulator

  // ✅ Sửa IP PC của bạn ở đây (ipconfig => IPv4)
  static const String _lanIp = 'http://192.168.1.15:5238/api'; // PC của bạn
  static const String _emulatorIp =
      'http://10.0.2.2:5238/api'; // Android emulator

  // Chọn URL phù hợp
  static String get baseUrl => runOnRealPhone ? _lanIp : _emulatorIp;

  // 🧩 Các getter "compat" để code hiện tại của bạn chạy được ngay
  static String get auth => '$baseUrl/auth';
  static String get users => '$baseUrl/users';

  // 🧩 (Tuỳ chọn) Endpoint cụ thể nếu muốn dùng trực tiếp
  static String get login => '$auth/login';
  static String get register => '$auth/register';
  static String get changePassword => '$auth/change-password';
  static String resetPassword(int id) => '$auth/users/$id/reset-password';
  static String get roleOptions => '$users/role-options';
  static String get patients => '$baseUrl/patients';
}
